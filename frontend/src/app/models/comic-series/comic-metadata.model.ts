import { PublicationStatus, Genre, Theme } from "../enums/comic-metadata.enum";

export interface ComicMetadata {
  title: string;
  author?: string;
  artist?: string;
  yearOfRelease?: number;
  writer?: string;
  penciler?: string;
  inker?: string;
  colorist?: string;
  description?: string;
  originalLanguage?: string;
  publicationStatus: PublicationStatus;
  genres: Genre[];
  themes: Theme[];
}