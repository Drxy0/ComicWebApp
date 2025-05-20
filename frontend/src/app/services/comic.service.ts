import { HttpClient } from "@angular/common/http";
import { inject, Injectable } from "@angular/core";

@Injectable({
    providedIn: 'root'
})
export class ComicService {
    private http = inject(HttpClient);
    private apiPath = 'http://localhost:5298';

    getComicSeries(id: string) {
        return this.http.get(`${this.apiPath}/comic-series/${id}`);
    }

}