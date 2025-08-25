"use client";

import React from "react";
import { Skeleton } from "@/components/ui/skeleton";

const MediaCardSkeleton = React.memo(() => (
    <div className="space-y-3">
        <Skeleton className="aspect-video w-full rounded-lg" />
        <div className="flex gap-3">
            <Skeleton className="h-9 w-9 rounded-full flex-shrink-0" />
            <div className="flex-1 space-y-2">
                <Skeleton className="h-4 w-full" />
                <Skeleton className="h-4 w-3/4" />
                <Skeleton className="h-3 w-1/2" />
            </div>
        </div>
    </div>
));

export default MediaCardSkeleton;
