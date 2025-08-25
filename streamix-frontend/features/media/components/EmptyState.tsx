import { Video } from "lucide-react";
import React from "react";

interface EmptyStateProps {
    type: "videos" | "streams";
}

const EmptyState = React.memo<EmptyStateProps>(({ type }) => (
    <div className="flex flex-col items-center justify-center py-16 px-4 text-center">
        <div className="rounded-full bg-muted p-4 mb-4">
            <Video className="h-8 w-8 text-muted-foreground" />
        </div>
        <h3 className="text-lg font-semibold mb-2">
            {type === "videos"
                ? "Here is no video yet"
                : "Here is no stream yet"}
        </h3>
        <p className="text-muted-foreground max-w-md">
            {type === "videos"
                ? "Video will appear here when creators upload content."
                : "Stream will appear here when streamers start streaming."}
        </p>
    </div>
));

export default EmptyState;
