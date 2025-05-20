import { HttpClient } from "@angular/common/http";
import { inject, Injectable } from "@angular/core";
import { ComicSeriesResponse } from "../models/comic-series/comic-series-response.model";

@Injectable({
    providedIn: 'root'
})
export class ComicService {
    private http = inject(HttpClient);
    private apiPath = 'http://localhost:5298';

    getComicSeries(id: string) {
        return this.http.get<ComicSeriesResponse>(`${this.apiPath}/comic-series/${id}`);
    }

}