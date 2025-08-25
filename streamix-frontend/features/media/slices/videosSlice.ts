import { createAsyncThunk, createSlice } from "@reduxjs/toolkit";
import { mockVideos } from "../constants/mockVideos";
import { VideosState } from "../types/VideosState";

const initialState: VideosState = {
    items: [],
    loading: false,
    error: null,
};

export const fetchVideos = createAsyncThunk("videos/fetchVideos", async () => {
    await new Promise((resolve) => setTimeout(resolve, 1000));
    return mockVideos;
});

const videosSlice = createSlice({
    name: "videos",
    initialState,
    reducers: {},
    extraReducers: (builder) => {
        builder
            .addCase(fetchVideos.pending, (state) => {
                state.loading = true;
                state.error = null;
            })
            .addCase(fetchVideos.fulfilled, (state, action) => {
                state.loading = false;
                state.items = action.payload;
            })
            .addCase(fetchVideos.rejected, (state, action) => {
                state.loading = false;
                state.error = action.error.message || "Failed to load videos";
            });
    },
});

export default videosSlice.reducer;
