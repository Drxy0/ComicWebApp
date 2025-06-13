import { Component, effect, ElementRef, HostListener, signal, viewChild, ViewChild } from '@angular/core';
import { ComicService } from '../../services/comic.service';
import { ActivatedRoute, Router } from '@angular/router';
import { DomSanitizer, SafeUrl } from '@angular/platform-browser';
import { TranslatePipe } from '@ngx-translate/core';

@Component({
  selector: 'app-reader',
  imports: [TranslatePipe],
  templateUrl: './reader.component.html',
  styleUrl: './reader.component.scss',
})
export class ReaderComponent {
  @ViewChild('page') page!: ElementRef<HTMLImageElement>;
  currentPageSpan = viewChild<ElementRef>('currentPageSpan');

  private imgWidth: number = 0;
  pageUrl: SafeUrl | null = null;
  pageCount = signal(0); // for navigation
  currentPage = signal(0);
  imageResolution = signal<{width: number, height: number} | null>(null);
  imageSize = signal<string>("");
  numberAndTitleHeading = signal<string>("");


  constructor(
    private comicService: ComicService,
    public route: ActivatedRoute,
    private router: Router,
    private sanitiser: DomSanitizer
  ) {
    const { chapterId, pageNumber } = this.route.snapshot.params

    this.route.params.subscribe(params => {
      this.currentPage.set(+params['pageNumber']);
    });
    
    this.comicService.getChapterNumberAndTitle(chapterId).subscribe((response) => {
      this.numberAndTitleHeading.set(`${response.number} - ${response.title}`)
    });

    this.comicService.getPage(chapterId, pageNumber).subscribe((blob: Blob) => {
      this.displayPage(blob);
    });

    this.comicService.getPageCount(chapterId).subscribe((response) => {
      this.pageCount.set((response as {pageCount: number}).pageCount);
    });

    effect(() => {
      const span = this.currentPageSpan();
      if (span) {
        span.nativeElement.textContent = 
          `${this.currentPage()} / ${this.pageCount()}`;
      }
    });
  }

  onImageLoad() {
    if (!this.page?.nativeElement) return;

    const img = this.page.nativeElement;
    this.imgWidth = img.getBoundingClientRect().width;
    
    this.imageResolution.set({
      width: img.naturalWidth,
      height: img.naturalHeight
    });
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
    const currentPage = pageNumber;
    const nextPage = currentPage - 1;

    if (nextPage <= 0 ) return;

    this.comicService.getPage(chapterId, nextPage).subscribe((blob: Blob) => {
      this.displayPage(blob);
      this.currentPage.set(nextPage);
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

    if (nextPage > this.pageCount()) return;

    this.comicService.getPage(chapterId, nextPage).subscribe((blob: Blob) => {
      this.displayPage(blob);
      this.currentPage.set(nextPage);
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
    this.imageSize.set((blob.size / (1024 * 1024)).toFixed(2)); 
    const objectUrl = URL.createObjectURL(blob);
    // Sanitize the URL for security - needed in prod
    this.pageUrl = this.sanitiser.bypassSecurityTrustUrl(objectUrl);
    this.imageResolution.set(null);
  }
}
