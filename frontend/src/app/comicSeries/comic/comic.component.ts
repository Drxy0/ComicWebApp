import { Component } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { ComicService } from '../../services/comic.service';

@Component({
  selector: 'app-comic',
  imports: [],
  templateUrl: './comic.component.html',
  styleUrl: './comic.component.scss'
})
export class ComicComponent {
  constructor(
    private route: ActivatedRoute,
    private comicService: ComicService
  ) {
    this.route.params.subscribe(params => {
      const id = params['id'];
      comicService.getComicSeries(id).subscribe({
        next: (data) => {
          console.log(data);
        },
        error: (err) => {
          console.log('Error');
        }
      })
    })
  }

  
}
