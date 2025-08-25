import { createAsyncThunk, createSlice } from "@reduxjs/toolkit";
import { mockStreams } from "../constants/mockStreams";
import { StreamsState } from "../types/StreamsState";

const initialState: StreamsState = {
    items: [],
    loading: false,
    error: null,
};

export const fetchStreams = createAsyncThunk(
    "streams/fetchStreams",
    async () => {
        await new Promise((resolve) => setTimeout(resolve, 800));
        return mockStreams;
    }
);

const streamsSlice = createSlice({
    name: "streams",
    initialState,
    reducers: {},
    extraReducers: (builder) => {
        builder
            .addCase(fetchStreams.pending, (state) => {
                state.loading = true;
                state.error = null;
            })
            .addCase(fetchStreams.fulfilled, (state, action) => {
                state.loading = false;
                state.items = action.payload;
            })
            .addCase(fetchStreams.rejected, (state, action) => {
                state.loading = false;
                state.error = action.error.message || "Failed to load streams";
            });
    },
});

export default streamsSlice.reducer;
