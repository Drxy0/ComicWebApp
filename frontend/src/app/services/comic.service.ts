import { HttpClient } from "@angular/common/http";
import { inject, Injectable } from "@angular/core";
import { ComicSeriesResponse } from "../models/comic-series/comic-series-response.model";
import { environment } from '../../environments/environment';

@Injectable({
    providedIn: 'root'
})
export class ComicService {
    private http = inject(HttpClient);
    private apiPath = environment.apiUrl;

    getComicSeries(id: string) {
        return this.http.get<ComicSeriesResponse>(`${this.apiPath}/comic-series/${id}`);
    }

    getCoverImage(id: string) {
        return this.http.get(`${this.apiPath}/comic-series/${id}/cover-image`, {
            responseType: 'blob'
        });
    }
}