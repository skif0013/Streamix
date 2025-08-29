import { createApi, fetchBaseQuery } from "@reduxjs/toolkit/query/react";

interface LoginRequest {
    email: string;
    password: string;
}

interface RegisterRequest {
    username: string;
    email: string;
    password: string;
    confirmPassword: string;
}

interface AuthResponse {
    token: string;
    refreshToken: string;
    expiresAt: string;
}

export const authApi = createApi({
    reducerPath: "authApi",
    baseQuery: fetchBaseQuery({
        baseUrl: "http://localhost:5001/api/Users",
        prepareHeaders: (headers) => {
            const token = localStorage.getItem("token");
            if (token) {
                headers.set("Authorization", `Bearer ${token}`);
            }
            return headers;
        },
    }),
    endpoints: (builder) => ({
        login: builder.mutation<AuthResponse, LoginRequest>({
            query: (credentials) => ({
                url: "/login",
                method: "POST",
                body: credentials,
            }),
            transformResponse: (response: AuthResponse) => {
                localStorage.setItem("token", response.token);
                localStorage.setItem("refreshToken", response.refreshToken);
                return response;
            },
        }),
        register: builder.mutation<{ message: string }, RegisterRequest>({
            query: (userData) => ({
                url: "/register",
                method: "POST",
                body: userData,
            }),
        }),
        forgotPassword: builder.mutation<
            { message: string },
            { email: string }
        >({
            query: (email) => ({
                url: "/forgotpassword",
                method: "POST",
                body: email,
            }),
        }),
        resetPassword: builder.mutation<
            { message: string },
            { email: string; newPassword: string }
        >({
            query: ({ email, newPassword }) => ({
                url: "/resetpassword",
                method: "POST",
                body: { email, newPassword },
            }),
        }),
        confirmEmail: builder.mutation<{ message: string }, { email: string }>({
            query: ({ email }) => ({
                url: "/confirmemail",
                method: "POST",
                body: { email },
            }),
        }),
    }),
});

export const {
    useLoginMutation,
    useRegisterMutation,
    useForgotPasswordMutation,
    useResetPasswordMutation,
    useConfirmEmailMutation,
} = authApi;
