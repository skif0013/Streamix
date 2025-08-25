import { Stream } from "./Stream";

export interface StreamsState {
    items: Stream[];
    loading: boolean;
    error: string | null;
}
