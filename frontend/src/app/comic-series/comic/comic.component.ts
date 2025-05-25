import { Component } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { ComicService } from '../../services/comic.service';
import { ComicSeriesResponse } from '../../models/comic-series/comic-series-response.model';

@Component({
  selector: 'app-comic',
  imports: [],
  templateUrl: './comic.component.html',
  styleUrl: './comic.component.scss'
})
export class ComicComponent {
  metadata: any;
  stats:any;
  chapters:any;

  constructor(
    private route: ActivatedRoute,
    private comicService: ComicService
  ) {
    this.route.params.subscribe(params => {
      const id = params['id'];
      comicService.getComicSeries(id).subscribe({
        next: (data: ComicSeriesResponse) => {
          this.metadata = data.metadata;
          this.stats = data.stats;
          this.chapters = data.chapters;

        },
        error: (err) => {
          console.log('Error');
        }
      })
    })
  }

  
}
