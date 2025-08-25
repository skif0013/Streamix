"use client";

import React, { useEffect } from "react";
import { Tabs, TabsContent, TabsList, TabsTrigger } from "@/components/ui/tabs";
import MediaGrid from "@/features/media/components/MediaGrid";
import { useAppDispatch, useAppSelector } from "@/lib/store";
import { fetchVideos, fetchStreams } from "@/features/media/slices";

const StreamixHomePage = React.memo(() => {
    const dispatch = useAppDispatch();

    const {
        items: videos,
        loading: isLoadingVideos,
        error: videoError,
    } = useAppSelector((state) => state.videos);

    const {
        items: streams,
        loading: isLoadingStreams,
        error: streamError,
    } = useAppSelector((state) => state.streams);

    useEffect(() => {
        dispatch(fetchVideos());
        dispatch(fetchStreams());
    }, [dispatch]);

    return (
        <div className="min-h-screen bg-background">
            <main className="container mx-auto">
                <Tabs defaultValue="videos" className="w-full">
                    <div className="sticky z-40 bg-background/80 backdrop-blur supports-[backdrop-filter]:bg-background/60">
                        <div className="container mx-auto p-2">
                            <TabsList className="relative flex w-full overflow-x-auto no-scrollbar justify-start rounded-xl border bg-muted/40 dark:bg-muted/20 backdrop-blur p-0 shadow-sm">
                                <TabsTrigger
                                    value="videos"
                                    className="relative whitespace-nowrap rounded-lg px-4 py-2 text-sm font-semibold text-muted-foreground transition-colors hover:text-foreground data-[state=active]:bg-background data-[state=active]:text-foreground data-[state=active]:shadow-sm"
                                >
                                    Video
                                    {!isLoadingVideos && (
                                        <span className="ml-2 rounded-full bg-muted/70 px-2 py-0.5 text-[10px] tracking-wide">
                                            {videos.length}
                                        </span>
                                    )}
                                </TabsTrigger>
                                <TabsTrigger
                                    value="streams"
                                    className="relative whitespace-nowrap rounded-lg px-4 py-2 text-sm font-semibold text-muted-foreground transition-colors hover:text-foreground data-[state=active]:bg-background data-[state=active]:text-foreground data-[state=active]:shadow-sm"
                                >
                                    Streams
                                    {!isLoadingStreams && (
                                        <span className="ml-2 rounded-full bg-muted/70 px-2 py-0.5 text-[10px] tracking-wide">
                                            {streams.length}
                                        </span>
                                    )}
                                    <span className="ml-2 relative inline-flex h-2 w-2">
                                        <span className="animate-ping absolute inline-flex h-2 w-2 rounded-full bg-red-500/60"></span>
                                        <span className="relative inline-flex h-2 w-2 rounded-full bg-red-500 shadow-[0_0_0_2px_rgba(239,68,68,0.25)]"></span>
                                    </span>
                                </TabsTrigger>
                            </TabsList>
                        </div>
                    </div>

                    <TabsContent value="videos" className="mt-0">
                        <MediaGrid
                            items={videos}
                            isLoading={isLoadingVideos}
                            isEmpty={videos.length === 0}
                            error={videoError}
                        />
                    </TabsContent>

                    <TabsContent value="streams" className="mt-0">
                        <MediaGrid
                            items={streams}
                            isLoading={isLoadingStreams}
                            isEmpty={streams.length === 0}
                            error={streamError}
                        />
                    </TabsContent>
                </Tabs>
            </main>
        </div>
    );
});

export default StreamixHomePage;
