import type { Metadata } from "next";
import "./globals.css";
import { Toaster } from "@/components/ui/sonner";
import ReduxProvider from "@/providers/ReduxProvider";
import Header from "@/layout/header";

export const metadata: Metadata = {
    title: "Streamix - Live Streaming & Video Platform | Watch & Stream Content",
    description:
        "Streamix is a next-generation streaming platform where creators and viewers connect. Watch live streams, follow your favorite streamers, and join a growing community of content creators. Start streaming today!",
    keywords: [
        "live streaming",
        "video platform",
        "stream games",
        "live video",
        "streaming service",
        "watch streams",
        "live broadcast",
        "gaming content",
        "esports",
        "streaming community",
        "go live",
        "content creators",
        "live events",
        "interactive streaming",
        "streaming platform",
    ],
    authors: [{ name: "Streamix Team" }],
    openGraph: {
        title: "Streamix - Where Streamers and Viewers Connect",
        description:
            "Join thousands of streamers and millions of viewers on Streamix - the ultimate live streaming platform for gaming, music, and creative content.",
        siteName: "Streamix",
        locale: "en_US",
        type: "website",
    },
    twitter: {
        card: "summary_large_image",
        title: "Streamix - The Future of Live Streaming",
        description:
            "Experience the next generation of live streaming with Streamix. Watch, stream, and connect with creators sworldwide.",
        creator: "@streamix",
    },
    alternates: {
        canonical: "http://localhost:3000",
    },
    category: "streaming",
    robots: {
        index: true,
        follow: true,
        googleBot: {
            index: true,
            follow: true,
            "max-video-preview": -1,
            "max-image-preview": "large",
            "max-snippet": -1,
        },
    },
};

export default function RootLayout({
    children,
}: Readonly<{
    children: React.ReactNode;
}>) {
    return (
        <html lang="en" suppressHydrationWarning>
            <body className="antialiased flex flex-col min-h-screen">
                <ReduxProvider>
                    <Header />
                    <main className="flex-1">{children}</main>
                    <Toaster />
                </ReduxProvider>
            </body>
        </html>
    );
}
