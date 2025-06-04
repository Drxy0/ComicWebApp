import { Component, ElementRef, HostListener, ViewChild } from '@angular/core';
import { ComicService } from '../../services/comic.service';
import { ActivatedRoute, Router } from '@angular/router';
import { DomSanitizer, SafeUrl } from '@angular/platform-browser';
import { Location } from '@angular/common';

@Component({
  selector: 'app-reader',
  imports: [],
  templateUrl: './reader.component.html',
  styleUrl: './reader.component.scss',
})
export class ReaderComponent {
  @ViewChild('page') page!: ElementRef<HTMLImageElement>;
  private imgWidth: number = 0;
  pageUrl: SafeUrl | null = null;
  pageCount: number = 0;

  constructor(
    private comicService: ComicService,
    private route: ActivatedRoute,
    private router: Router,
    private location: Location,
    private sanitiser: DomSanitizer
  ) {
    const { chapterId, pageNumber } = this.route.snapshot.params;

    this.comicService.getPage(chapterId, pageNumber).subscribe((blob: Blob) => {
      this.displayPage(blob);
    });

    this.comicService.getPageCount(chapterId).subscribe((response) => {
      this.pageCount = (response as { pageCount: number }).pageCount;
    });
  }

  onImageLoad() {
    if (!this.page?.nativeElement) return;
    this.imgWidth = this.page.nativeElement.getBoundingClientRect().width;
  }

  handlePageClick(event: MouseEvent | TouchEvent) {
    if (!this.page?.nativeElement) return;

    const clientX =
      'touches' in event ? event.touches[0].clientX : event.clientX;

    const rect = this.page.nativeElement.getBoundingClientRect();
    const x = clientX - rect.left;

    const thirdWidth = this.imgWidth / 3;
    const isCenterClick = x > thirdWidth && x < thirdWidth * 2;

    if (x < thirdWidth) {
      this.goToPrevPage();
    } else if (x > thirdWidth * 2) {
      this.goToNextPage();
    } else if (isCenterClick) {
      this.showMenu();
    }
  }

  @HostListener('window:resize')
  onResize() {
    this.onImageLoad();
  }

  goToPrevPage() {
    const { chapterId, pageNumber } = this.route.snapshot.params;
    const currentPage = +pageNumber;
    const nextPage = currentPage - 1;

    if (nextPage <= 0 ) return;

    this.comicService.getPage(chapterId, nextPage).subscribe((blob: Blob) => {
      this.displayPage(blob);

      this.router.navigate(['../', nextPage], {
        relativeTo: this.route,
        replaceUrl: true,
        skipLocationChange: false,
        state: { skipPageLoad: true },
      });
    });
  }

  goToNextPage() {
    const { chapterId, pageNumber } = this.route.snapshot.params;
    const currentPage = +pageNumber;
    const nextPage = currentPage + 1;

    if (nextPage > this.pageCount) return;

    this.comicService.getPage(chapterId, nextPage).subscribe((blob: Blob) => {
      this.displayPage(blob);

      this.router.navigate(['../', nextPage], {
        relativeTo: this.route,
        replaceUrl: true,
        skipLocationChange: false,
        state: { skipPageLoad: true },
      });
    });
  }

  showMenu() {
    console.log('menu');
  }

  displayPage(blob: Blob) {
    const objectUrl = URL.createObjectURL(blob);
    // Sanitize the URL for security - needed in prod
    this.pageUrl = this.sanitiser.bypassSecurityTrustUrl(objectUrl);
  }
}
