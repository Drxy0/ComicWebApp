export interface Chapter {
  title?: string;
  number: number;
  id: string;
  seriesId: string;
  pages: ChapterFile[];
}

export interface ChapterFile {
  id: string;
  pageNumber: number;
}
