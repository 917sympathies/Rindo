import type { Metadata } from "next";
import { Noto_Sans } from "next/font/google";
import "./globals.css";
import { ThemeProvider } from "@/components/providers/themeProvider";
import React from "react";

const noto_sans = Noto_Sans({ subsets: ["latin", "cyrillic"] });

export const metadata: Metadata = {
    title: "Rindo",
    description: "App to manage your projects!",
};

export default function RootLayout({ children }: Readonly<{ children: React.ReactNode }>) {
    return (
        <html lang="en" suppressHydrationWarning>
        <body className={noto_sans.className}>
        <ThemeProvider
            attribute="class"
            defaultTheme="light"
            enableSystem
            disableTransitionOnChange
        >
            {children}
        </ThemeProvider>
        </body>
        </html>
    );
}
