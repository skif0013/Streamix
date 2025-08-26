"use client";

import React from "react";
import MediaCard from "@/features/media/components/MediaCard";
import { MediaGridProps } from "@/features/media/types/MediaGridProps";
import { ErrorState, EmptyState } from "@/features/media/components/index";
import MediaCardSkeleton from "@/features/media/components/MediaCardSkeleton";

const MediaGrid = React.memo<MediaGridProps>(
    ({ items, isLoading = false, isEmpty = false, error = null }) => {
        if (error) {
            return <ErrorState error={error} />;
        }
        if (isEmpty && !isLoading) {
            const hasVideos = items.some((item) => item.type === "video");
            const hasStreams = items.some((item) => item.type === "stream");

            if (!hasVideos && !hasStreams) {
                return <EmptyState type="videos" />;
            }
        }

        return (
            <div className="px-4 py-2">
                <div className="grid grid-cols-1 sm:grid-cols-2 lg:grid-cols-3 xl:grid-cols-4 gap-4 ">
                    {isLoading
                        ? Array.from({ length: 12 }).map((_, index) => (
                              <MediaCardSkeleton key={`skeleton-${index}`} />
                          ))
                        : items.map((item) => (
                              <MediaCard key={item.id} item={item} />
                          ))}
                </div>
            </div>
        );
    }
);

export default MediaGrid;
