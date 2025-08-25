import React from "react";
import { Alert, AlertDescription } from "@/components/ui/alert";
import { AlertCircle } from "lucide-react";

interface ErrorStateProps {
    error: string;
}

const ErrorState = React.memo<ErrorStateProps>(({ error }) => (
    <Alert variant="destructive" className="mx-4">
        <AlertCircle className="h-4 w-4" />
        <AlertDescription>Downloading error: {error}</AlertDescription>
    </Alert>
));

export default ErrorState;
