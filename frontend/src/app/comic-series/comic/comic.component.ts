import { Component, OnDestroy } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { ComicService } from '../../services/comic.service';
import { ComicSeriesResponse } from '../../models/comic-series/comic-series-response.model';
import { TranslatePipe } from '@ngx-translate/core';
import { catchError, of, Subject, Subscription, switchMap, takeUntil, tap } from 'rxjs';
import { ComicStats } from '../../models/comic-series/comic-stats.model';
import { PublicationStatus } from '../../models/enums/comic-metadata.enum';
import { LanguageFlagPipe } from '../../core/pipes/language-flag.pipe';

@Component({
  selector: 'app-comic',
  imports: [TranslatePipe, LanguageFlagPipe],
  templateUrl: './comic.component.html',
  styleUrl: './comic.component.scss'
})
export class ComicComponent implements OnDestroy {
  private defaultCoverImage = 'assets/default-cover.webp';
  
  comicData!: ComicSeriesResponse;
  coverImageUrl: string = this.defaultCoverImage;
  getComicData$!: Subscription;
  chaptersSortOrder: 'asc' | 'desc' = 'asc';

  constructor(
    private route: ActivatedRoute,
    private comicService: ComicService
  ) {
    this.comicData = {
      id: '',
      isVerified: false,
      metadata: {
        title: 'Loading...',
        publicationStatus: PublicationStatus.Ongoing,
        genres: [],
        themes: [],
        description: ''
      },
      stats: {} as ComicStats,
      chapters: []
    };
    this.getComicData$ = this.route.params.pipe(
      switchMap(params => {
        return this.comicService.getComicSeries(params['id']).pipe(
          tap((data: ComicSeriesResponse) => {
            this.comicData = data;
            this.sortChapters();
          }),
          switchMap(() => this.loadCoverImage(params['id'])),
          catchError(err => {
            console.error('Error loading comic series:', err);
            return of(null);
          })
        );
      })
      ).subscribe({
        next: (imageBlob: Blob | null) => {
          this.handleCoverImage(imageBlob);
        },
        error: (err) => {
          console.error('Overall error:', err);
          this.setDefaultCoverImage();
        }
      });
  }

  ngOnDestroy(): void {
    this.cleanUpCoverImageUrl();
  }

  private loadCoverImage(id: string) {
    return this.comicService.getCoverImage(id).pipe(
      catchError(err => {
        console.error('Error loading cover image:', err);
        return of(null);
      })
    );
  }

  private handleCoverImage(imageBlob: Blob | null): void {
    this.cleanUpCoverImageUrl();
    
    if (imageBlob) {
      this.coverImageUrl = URL.createObjectURL(imageBlob);
    } else {
      this.setDefaultCoverImage();
    }
  }

  private setDefaultCoverImage(): void {
    this.coverImageUrl = this.defaultCoverImage;
  }

  private cleanUpCoverImageUrl(): void {
    if (this.coverImageUrl && this.coverImageUrl.startsWith('blob:')) {
      URL.revokeObjectURL(this.coverImageUrl);
    }
  }

sortChapters() {
  if (this.comicData?.chapters) {
    this.comicData.chapters.sort((a, b) => {
      if (this.chaptersSortOrder === 'asc') {
        return a.number - b.number;
      } else {
        return b.number - a.number;
      }
    });
    
    this.chaptersSortOrder = this.chaptersSortOrder === 'asc' ? 'desc' : 'asc';
  }
}
}
