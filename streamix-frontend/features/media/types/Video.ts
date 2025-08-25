import { BaseMedia } from "./BaseMedia";

export interface Video extends BaseMedia {
    type: "video";
    uploadedAt: string;
    duration: string;
}
