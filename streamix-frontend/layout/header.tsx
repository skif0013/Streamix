"use client";

import { useState } from "react";
import { Search, Plus } from "lucide-react";
import Link from "next/link";
import { Button } from "../components/ui/button";
import { Input } from "../components/ui/input";
import { UserDropdownMenu } from "./components/UserDropdownMenu";
import { AuthModal } from "@/features/auth/components/AuthModal";

const Header = () => {
    const [searchQuery, setSearchQuery] = useState("");
    const [isLoggedIn, setIsLoggedIn] = useState(false);

    const user = {
        name: "John Doe",
        email: "john.doe@example.com",
        avatar: "",
    };

    return (
        <header className="sticky top-0 z-50 w-full bg-background px-4">
            <div className="container flex h-16 items-center justify-between">
                <div className="items-center hidden md:flex">
                    <Link href="/" className="mr-6 flex items-center space-x-2">
                        <span className="text-xl font-bold text-black">
                            Streamix
                        </span>
                    </Link>
                </div>

                <div className="flex flex-1 items-center justify-center">
                    <div className="relative w-full max-w-2xl">
                        <Input
                            type="search"
                            placeholder="Search videos..."
                            className="w-full rounded-r-none focus-visible:ring-1 focus-visible:ring-offset-0"
                            value={searchQuery}
                            onChange={(e) => setSearchQuery(e.target.value)}
                        />
                        <Button
                            type="submit"
                            size="icon"
                            className="absolute right-0 top-0 h-full rounded-l-none rounded-r-md"
                        >
                            <Search className="h-4 w-4" />
                            <span className="sr-only">Search</span>
                        </Button>
                    </div>
                </div>

                <div className="flex items-center pl-2 space-x-2 md:space-x-6">
                    <Button
                        variant="outline"
                        size="icon"
                        className="rounded-full"
                    >
                        <Plus className="h-5 w-5" />
                        <span className="sr-only">Create</span>
                    </Button>

                    {isLoggedIn ? (
                        <UserDropdownMenu
                            user={user}
                            setIsLoggedIn={setIsLoggedIn}
                        />
                    ) : (
                        <AuthModal />
                    )}
                </div>
            </div>
        </header>
    );
};

export default Header;
