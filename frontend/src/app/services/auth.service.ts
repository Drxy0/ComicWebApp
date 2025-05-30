import { inject, Injectable } from "@angular/core";
import { HttpClient } from '@angular/common/http';
import { RegisterUserDto } from "../models/auth/register-user.dto";
import { LoginUserDto } from "../models/auth/login-user.dto";
import { RefreshTokenDto } from "../models/auth/refresh-token.dto";
import { Observable } from "rxjs";
import { environment } from "../../environments/environment";

@Injectable({
    providedIn: 'root'
})
export class AuthService {
    private http = inject(HttpClient);
    private apiPath = environment.apiUrl;

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