import { useDispatch } from "react-redux";
import { useLoginMutation, useRegisterMutation } from "../api/authApi";
import { logout, setError, setLoading } from "../slices/authSlice";
import { useRouter } from "next/navigation";
import { toast } from "sonner";
import { AppDispatch } from "@/lib/store";

export const useAuth = () => {
    const dispatch = useDispatch<AppDispatch>();
    const router = useRouter();
    const [login] = useLoginMutation();
    const [register] = useRegisterMutation();

    const handleLogin = async (email: string, password: string) => {
        try {
            dispatch(setLoading(true));
            await login({
                email,
                password,
            }).unwrap();

            toast.success("Successfully logged in!");
        } catch (error: any) {
            const errorMessage =
                error.data?.message || "Login failed. Please try again.";
            dispatch(setError(errorMessage));
            toast.error(errorMessage);
        } finally {
            dispatch(setLoading(false));
        }
    };

    const handleRegister = async (userData: {
        username: string;
        email: string;
        password: string;
        confirmPassword: string;
    }) => {
        try {
            dispatch(setLoading(true));
            await register(userData);

            toast.success(
                "Registration successful! Please check your email to verify your account."
            );
        } catch (error: any) {
            const errorMessage =
                error.data?.message || "Registration failed. Please try again.";
            dispatch(setError(errorMessage));
            toast.error(errorMessage);
        } finally {
            dispatch(setLoading(false));
        }
    };

    const handleLogout = () => {
        try {
            dispatch(setLoading(true));
            dispatch(logout());
            router.push("/");
            toast.success("Successfully logged out");
        } catch (error) {
            dispatch(setError("Failed to log out"));
            toast.error("Failed to log out");
        } finally {
            dispatch(setLoading(false));
        }
    };

    return {
        handleLogin,
        handleRegister,
        handleLogout,
    };
};
