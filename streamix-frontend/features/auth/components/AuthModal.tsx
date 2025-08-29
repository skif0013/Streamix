"use client";

import { useState } from "react";
import { Button } from "@/components/ui/button";
import {
    Dialog,
    DialogContent,
    DialogDescription,
    DialogHeader,
    DialogTitle,
} from "@/components/ui/dialog";
import { Tabs, TabsContent, TabsList, TabsTrigger } from "@/components/ui/tabs";
import { LoginForm } from "./LoginForm";
import { RegisterForm } from "./RegisterForm";

export const AuthModal = () => {
    const [isOpen, setIsOpen] = useState(false);
    const [activeTab, setActiveTab] = useState("login");

    return (
        <>
            <Button onClick={() => setIsOpen(true)}>Sign In</Button>
            <Dialog open={isOpen} onOpenChange={setIsOpen}>
                <DialogContent className="sm:max-w-[425px] py-6">
                    <Tabs
                        value={activeTab}
                        onValueChange={setActiveTab}
                        className="w-full"
                    >
                        <TabsList className="grid w-full grid-cols-2">
                            <TabsTrigger value="login">Sign In</TabsTrigger>
                            <TabsTrigger value="register">
                                Create Account
                            </TabsTrigger>
                        </TabsList>

                        <TabsContent value="login">
                            <DialogHeader>
                                <DialogTitle>Welcome back</DialogTitle>
                                <DialogDescription>
                                    Sign in to access your account and continue
                                    watching.
                                </DialogDescription>
                            </DialogHeader>
                            <LoginForm />
                        </TabsContent>

                        <TabsContent value="register">
                            <DialogHeader>
                                <DialogTitle>Create an account</DialogTitle>
                                <DialogDescription>
                                    Join Streamix to start watching and sharing
                                    your favorite content.
                                </DialogDescription>
                            </DialogHeader>
                            <RegisterForm
                                onSuccess={() => {
                                    setActiveTab("login");
                                }}
                            />
                        </TabsContent>
                    </Tabs>
                </DialogContent>
            </Dialog>
        </>
    );
};
