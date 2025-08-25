"use client";

import React from "react";
import Image from "next/image";
import { Card, CardContent } from "@/components/ui/card";
import { Avatar, AvatarFallback, AvatarImage } from "@/components/ui/avatar";
import { Badge } from "@/components/ui/badge";
import { formatViews, formatTimeAgo } from "@/features/media/utils";
import { MediaItem } from "@/features/media/types/MediaItem";

export interface MediaCardProps {
    item: MediaItem;
}

const MediaCard = React.memo<MediaCardProps>(({ item }) => {
    const isStream = item.type === "stream";
    const channelInitials = item.channelName.slice(0, 2).toUpperCase();

    return (
        <Card className="group cursor-pointer transition-all py-0 pb-4 duration-200 hover:shadow-lg border-0 bg-transparent">
            <CardContent className="p-0 space-y-3">
                <div className="relative aspect-video overflow-hidden rounded-t-lg bg-muted">
                    <Image
                        src={item.thumbnail}
                        alt={item.title}
                        fill
                        className="object-cover transition-transform duration-200 group-hover:scale-105"
                        sizes="(max-width: 640px) 100vw, (max-width: 1024px) 33vw, 25vw"
                    />
                    <div className="absolute bottom-2 right-2">
                        {isStream ? (
                            <Badge
                                variant="destructive"
                                className="bg-red-600 hover:bg-red-600 text-white font-semibold px-2 py-1"
                            >
                                LIVE
                            </Badge>
                        ) : (
                            <Badge
                                variant="secondary"
                                className="bg-black/80 text-white hover:bg-black/80 font-medium px-2 py-1"
                            >
                                {item.duration}
                            </Badge>
                        )}
                    </div>
                    {isStream && (
                        <div className="absolute top-2 left-2">
                            <Badge
                                variant="secondary"
                                className="bg-black/80 text-white hover:bg-black/80 font-medium px-2 py-1"
                            >
                                {formatViews(item.viewers)} viewers
                            </Badge>
                        </div>
                    )}
                </div>
                <div className="flex gap-3 px-2">
                    <Avatar className="h-9 w-9 flex-shrink-0">
                        <AvatarImage
                            src={item.channelAvatar}
                            alt={item.channelName}
                            className="object-cover"
                        />
                        <AvatarFallback className="text-xs font-semibold">
                            {channelInitials}
                        </AvatarFallback>
                    </Avatar>
                    <div className="flex-1 min-w-0">
                        <h3
                            className="font-semibold text-sm leading-5 line-clamp-2 group-hover:text-primary transition-colors"
                            title={item.title}
                        >
                            {item.title}
                        </h3>

                        <p className="text-muted-foreground text-sm mt-1 hover:text-foreground transition-colors cursor-pointer">
                            {item.channelName}
                        </p>
                        <div className="flex items-center text-muted-foreground text-sm mt-1">
                            <span>{formatViews(item.views)} views</span>
                            {!isStream ? (
                                <>
                                    <span className="mx-1">•</span>
                                    <span>
                                        {formatTimeAgo(item.uploadedAt)}
                                    </span>
                                </>
                            ) : (
                                <div className="mt-1">
                                    <Badge
                                        variant="outline"
                                        className="text-xs ml-1 px-2 py-0.5"
                                    >
                                        {item.category}
                                    </Badge>
                                </div>
                            )}
                        </div>
                    </div>
                </div>
            </CardContent>
        </Card>
    );
});

export default MediaCard;
