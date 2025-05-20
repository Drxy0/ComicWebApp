import { Chapter } from "./chapter.model";
import { ComicMetadata } from "./comic-metadata.model";
import { ComicStats } from "./comic-stats.model";

export interface ComicSeriesResponse {
  id: string;
  metadata: ComicMetadata;
  stats?: ComicStats;
  chapters?: Chapter[];
  isVerified: boolean;
}