import { Component, OnDestroy } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { ComicService } from '../../services/comic.service';
import { ComicSeriesResponse } from '../../models/comic-series/comic-series-response.model';
import { ComicMetadata } from '../../models/comic-series/comic-metadata.model';
import { ComicStats } from '../../models/comic-series/comic-stats.model';
import { ComicChapter } from '../../models/comic-series/comic-chapter.model';
import { TranslatePipe } from '@ngx-translate/core';
import { catchError, of, Subject, switchMap, takeUntil, tap } from 'rxjs';

@Component({
  selector: 'app-comic',
  imports: [TranslatePipe],
  templateUrl: './comic.component.html',
  styleUrl: './comic.component.scss'
})
export class ComicComponent implements OnDestroy {
  metadata!: ComicMetadata;
  stats!: ComicStats;
  chapters: ComicChapter[] = [];
  coverImageUrl: string | null = null;
  
  private destroy$ = new Subject<void>();
  private defaultCoverImage = 'assets/default-cover.webp';

  constructor(
    private route: ActivatedRoute,
    private comicService: ComicService
  ) {
    this.loadComicData();
  }

  ngOnDestroy(): void {
    this.cleanUpCoverImageUrl();
    this.destroy$.next();
    this.destroy$.complete();
  }

  private loadComicData(): void {
    this.route.params.pipe(
      switchMap(params => {
        const id = params['id'];
        return this.comicService.getComicSeries(id).pipe(
          tap((data: ComicSeriesResponse) => {
            this.metadata = data.metadata;
            this.stats = data.stats ?? {} as ComicStats;
            this.chapters = data.chapters ?? [];
          }),
          switchMap(() => this.loadCoverImage(id)),
          catchError(err => {
            console.error('Error loading comic series:', err);
            return of(null);
          })
        );
      }),
      takeUntil(this.destroy$)
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
}
