import { CanActivateFn, Router } from "@angular/router";
import { inject } from "@angular/core";
import { AuthService } from "../../services/auth.service";

export const isAuthenticatedGuard = (): CanActivateFn => {
    return () => {
        const authService = inject(AuthService);
        const router = inject(Router);

        if (authService.getUserAccessToken()) {
            return true;
        }

        return router.parseUrl('login');
    }
}