import { ComicChapter } from "./comic-chapter.model";
import { ComicMetadata } from "./comic-metadata.model";
import { ComicStats } from "./comic-stats.model";

export interface ComicSeriesResponse {
  id: string;
  metadata: ComicMetadata;
  stats?: ComicStats;
  chapters?: ComicChapter[];
  isVerified: boolean;
}