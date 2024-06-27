"use client"
 
import * as React from "react"
import { Moon, Sun } from "lucide-react"
import { useTheme } from "next-themes"
import { useState } from "react"
 
import { Button } from "@/components/ui/button"
 
export function ModeToggle() {
  const { setTheme } = useTheme()
  const [isLight, setIsLight] = useState<boolean>(true);

  const handleChangeTheme = () => {
    if(isLight){
        setTheme("dark");
        setIsLight(false);
    }
    else{
        setTheme("light")
        setIsLight(true);
    }
  }

  return (
        <Button variant="ghost" size="icon" onClick={() => handleChangeTheme()}>
          <Sun className="h-[1rem] w-[1rem] rotate-0 scale-100 transition-all dark:-rotate-90 dark:scale-0" />
          <Moon className="absolute h-[1rem] w-[1rem] rotate-90 scale-0 transition-all dark:rotate-0 dark:scale-100" />
          <span className="sr-only">Toggle theme</span>
        </Button>
  )
}