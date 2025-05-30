export interface ComicChapter {
  id: string;
  seriesId: string;
  title?: string;
  number: number;
  language: string;
  pages: ChapterFile[];
}

export interface ChapterFile {
  id: string;
  pageNumber: number;
}
