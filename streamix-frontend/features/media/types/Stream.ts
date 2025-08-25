import { BaseMedia } from "./BaseMedia";

export interface Stream extends BaseMedia {
    type: "stream";
    isLive: boolean;
    category: string;
    viewers: number;
}
