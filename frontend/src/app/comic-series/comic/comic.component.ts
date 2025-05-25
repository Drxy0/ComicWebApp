import { Component } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { ComicService } from '../../services/comic.service';
import { ComicSeriesResponse } from '../../models/comic-series/comic-series-response.model';
import { ComicMetadata } from '../../models/comic-series/comic-metadata.model';
import { ComicStats } from '../../models/comic-series/comic-stats.model';
import { ComicChapter } from '../../models/comic-series/comic-chapter.model';
import { TranslatePipe } from '@ngx-translate/core';

@Component({
  selector: 'app-comic',
  imports: [TranslatePipe],
  templateUrl: './comic.component.html',
  styleUrl: './comic.component.scss'
})
export class ComicComponent {
  metadata!: ComicMetadata;
  stats!: ComicStats;
  chapters!: ComicChapter[];

  constructor(
    private route: ActivatedRoute,
    private comicService: ComicService
  ) {
    this.route.params.subscribe(params => {
      const id = params['id'];
      comicService.getComicSeries(id).subscribe({
        next: (data: ComicSeriesResponse) => {
          this.metadata = data.metadata;
          this.stats = data.stats ?? {} as ComicStats;
          this.chapters = data.chapters ?? [];

          console.log(this.metadata);
        },
        error: (err) => {
          console.log('Error');
        }
      })
    })
  }

  
}
