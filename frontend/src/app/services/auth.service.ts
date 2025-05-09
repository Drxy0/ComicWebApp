import { inject, Injectable } from "@angular/core";
import { HttpClient } from '@angular/common/http';
import { RegisterUserDto } from "../models/dtos/register-user.dto";
import { LoginUserDto } from "../models/dtos/login-user.dto";
import { RefreshTokenDto } from "../models/dtos/refresh-token.dto";
import { Observable } from "rxjs";

@Injectable({
    providedIn: 'root'
})
export class AuthService {
    private http = inject(HttpClient);
    private apiPath = 'http://localhost:5298'; // QUESTION: Where do i place this, some config or env file mby?

    getUserAccessToken() {
        const accessToken = localStorage.getItem('access_token');

        if (!accessToken) return null;

        const isValid = this.isTokenValid(accessToken);

        if (isValid) {
            return accessToken;
        }

        return null;
    }

    private isTokenValid(token: string): boolean {
        try {
            const payload = JSON.parse(atob(token.split('.')[1]));
            const exp = payload.exp * 1000;
            return Date.now() < exp;  // Check if token is expired
        } catch (ex) {
            return false;
        }
    }

    register(registerUser: RegisterUserDto): Observable<any> {
        return this.http.post(`${this.apiPath}/auth/register`, registerUser);
    }

    login(loginUser: LoginUserDto): Observable<any> {
        return this.http.post(`${this.apiPath}/auth/login`, loginUser);
    }

    refreshToken(refreshDto: RefreshTokenDto): Observable<any> {
        return this.http.post(`${this.apiPath}/auth/refresh-token`, refreshDto);
    }
}