import { MediaItem } from "../types/MediaItem";
import { mockVideos, mockStreams } from "../constants";

export const getAllMedia = (): MediaItem[] => [...mockVideos, ...mockStreams];
