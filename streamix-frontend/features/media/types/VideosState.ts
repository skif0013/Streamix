import { Video } from "./Video";

export interface VideosState {
    items: Video[];
    loading: boolean;
    error: string | null;
}
