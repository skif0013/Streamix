import { Avatar, AvatarFallback } from "@radix-ui/react-avatar";
import {
    DropdownMenu,
    DropdownMenuContent,
    DropdownMenuItem,
    DropdownMenuTrigger,
} from "@radix-ui/react-dropdown-menu";
import { Button } from "@/components/ui/button";
import { User } from "lucide-react";

interface UserDropdownMenuProps {
    user: {
        name: string;
        email: string;
        avatar: string;
    };
    setIsLoggedIn: (isLoggedIn: boolean) => void;
}

export const UserDropdownMenu = ({
    user,
    setIsLoggedIn,
}: UserDropdownMenuProps) => {
    return (
        <DropdownMenu>
            <DropdownMenuTrigger asChild>
                <Button
                    variant="ghost"
                    className="relative h-10 w-10 rounded-full"
                >
                    <Avatar className="h-10 w-10">
                        <AvatarFallback>
                            {user.name
                                .split(" ")
                                .map((n) => n[0])
                                .join("")}
                        </AvatarFallback>
                    </Avatar>
                </Button>
            </DropdownMenuTrigger>
            <DropdownMenuContent className="w-72 p-0" align="end" forceMount>
                <div className="p-4 border-b">
                    <div className="flex items-center space-x-3">
                        <div className="relative">
                            <Avatar className="h-10 w-10">
                                <AvatarFallback>
                                    {user.name
                                        .split(" ")
                                        .map((n) => n[0])
                                        .join("")}
                                </AvatarFallback>
                            </Avatar>
                        </div>
                        <div className="space-y-1">
                            <p className="text-sm font-medium">{user.name}</p>
                            <p className="text-xs text-muted-foreground">
                                {user.email}
                            </p>
                        </div>
                    </div>
                </div>
                <div className="p-1">
                    <DropdownMenuItem className="px-3 py-2 text-sm">
                        <User className="mr-2 h-4 w-4" />
                        <span>Your Channel</span>
                    </DropdownMenuItem>
                    <DropdownMenuItem className="px-3 py-2 text-sm">
                        <span>Your Videos</span>
                    </DropdownMenuItem>
                    <DropdownMenuItem className="px-3 py-2 text-sm">
                        <span>Settings</span>
                    </DropdownMenuItem>
                    <div className="border-t my-1" />
                    <DropdownMenuItem
                        className="px-3 py-2 text-sm"
                        onClick={() => setIsLoggedIn(false)}
                    >
                        <span>Sign out</span>
                    </DropdownMenuItem>
                </div>
            </DropdownMenuContent>
        </DropdownMenu>
    );
};
