package com.github.auth.domain.object.account.dto;

import io.swagger.v3.oas.annotations.media.Schema;
import lombok.AllArgsConstructor;
import lombok.Builder;
import lombok.Data;
import org.springframework.http.HttpStatus;

@AllArgsConstructor
@Schema(description = "Response for create account")
@Builder
@Data
public class AuthResponse {
    @Schema(description = "code status", example = "200")
    private HttpStatus status;

    @Schema(description = "message", example = "message")
    private String message;

    @Schema(description = "access token",
            example = "eyJhbGciOiJIUzI1NiJ9.eyJyb2xlIjpbeyJhdXRogur5s")
    private String accessToken;

    @Schema(description = "refreshToken",
            example = "eyJhbGciOiJIUzI1NiJ9.eyJyb2xlIjpbeyJhdXRob3Jpd")
    private String refreshToken;

    private UserInfoObject userInfoObject;

    public AuthResponse(HttpStatus status, String message) {
        this.status = status;
        this.message = message;
    }
}