import { MediaItem } from "./MediaItem";

export interface MediaGridProps {
    items: MediaItem[];
    isLoading?: boolean;
    isEmpty?: boolean;
    error?: string | null;
}
