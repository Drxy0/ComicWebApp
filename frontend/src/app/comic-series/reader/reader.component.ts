import { Component, effect, ElementRef, HostListener, inject, OnInit, signal, viewChild, ViewChild, DestroyRef } from '@angular/core';
import { ComicService } from '../../services/comic.service';
import { ActivatedRoute, Router } from '@angular/router';
import { DomSanitizer, SafeUrl } from '@angular/platform-browser';
import { TranslatePipe } from '@ngx-translate/core';
import { ImageDimensions } from '../../models/interfaces/image-dimensions.interface';
import { switchMap, tap } from 'rxjs';
import { takeUntilDestroyed } from '@angular/core/rxjs-interop';

@Component({
  selector: 'app-reader',
  imports: [TranslatePipe],
  templateUrl: './reader.component.html',
  styleUrl: './reader.component.scss',
})
export class ReaderComponent {
  @ViewChild('page') page!: ElementRef<HTMLImageElement>;
  currentPageSpan = viewChild<ElementRef>('currentPageSpan');
  
  private destroyRef = inject(DestroyRef);

  private imgWidth: number = 0;
  pageUrl: SafeUrl | null = null;
  pageCount = signal(0); // for navigation
  currentPage = signal(0);
  imageResolution = signal<ImageDimensions | null>(null);
  imageSize = signal<string>("");
  numberAndTitleHeading = signal<string>("");

  
  constructor(
    private comicService: ComicService,
    public route: ActivatedRoute,
    private router: Router,
    private sanitiser: DomSanitizer
  ) { }

    ngOnInit() {
    const param$ = this.route.params.pipe(
      takeUntilDestroyed(this.destroyRef)
    );

    // Handle current page and page content
    param$
      .pipe(
        tap(params => this.currentPage.set(+params['pageNumber'])),
        switchMap(params => 
          this.comicService.getPage(params['chapterId'], +params['pageNumber'])
        )
      )
      .subscribe(blob => this.displayPage(blob));

    // Chapter title
    param$
      .pipe(
        switchMap(params => this.comicService.getChapterNumberAndTitle(params['chapterId']))
      )
      .subscribe(res => {
        this.numberAndTitleHeading.set(`${res.number} - ${res.title}`);
      });

    // Page count
    param$
      .pipe(
        switchMap(params => this.comicService.getPageCount(params['chapterId']))
      )
      .subscribe(res => {
        this.pageCount.set((res as { pageCount: number }).pageCount);
      });

    // Display text like "3 / 10"
    effect(() => {
      const span = this.currentPageSpan();
      if (span) {
        span.nativeElement.textContent = `${this.currentPage()} / ${this.pageCount()}`;
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