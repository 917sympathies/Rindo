"use client";
import { Label } from "@/components/ui/label";
import Link from "next/link"
import { useState, useEffect } from "react";
import { redirect, useParams, useRouter, usePathname } from "next/navigation";
import {
  Drama,
  Settings2,
  Users,
} from "lucide-react";

interface Props {
    children: React.ReactNode;
  }

export default function Layout({children} : Props) {
  const { id } = useParams<{ id: string }>();
  const pathname = usePathname();
  const [currentSetting, setCurrentSetting] = useState<string>();
  

  useEffect(() => {
    const arr = pathname.split("/");
    setCurrentSetting(arr[arr.length-1]);
  }, [pathname])

  return (
    <div className="grid grid-cols-[1fr_4fr]">
      <div className="flex flex-col items-start pt-[2rem] pl-[2rem]">
        <Link
          href={`general`}
          className={
            currentSetting === "general"
              ? "text-none hover:cursor-pointer text-white flex flex-row items-center gap-[0.5em] w-[80%] text-[1rem] font-normal my-[0.5rem] mx-0 py-[0.4rem] px-[0.8rem] rounded-lg bg-[#0077b6]"
              : "text-none hover:cursor-pointer text-white flex flex-row items-center gap-[0.5em] w-[80%] text-[1rem] font-normal my-[0.5rem] mx-0 py-[0.4rem] px-[0.8rem] rounded-lg bg-[#0077b6] hover:bg-[#0074b3]"
          }
        >
          <Settings2 size={18} />
          <Label style={{ font: "inherit" }}>Общие</Label>
        </Link>
        <Link
         href={`users`}
          className={
            currentSetting === "users"
              ? "hover:cursor-pointer text-white flex flex-row items-center gap-[0.5em] w-[80%] text-[1rem] font-normal my-[0.5rem] mx-0 py-[0.4rem] px-[0.8rem] rounded-lg bg-[#0096c7]"
              : "hover:cursor-pointer text-white flex flex-row items-center gap-[0.5em] w-[80%] text-[1rem] font-normal my-[0.5rem] mx-0 py-[0.4rem] px-[0.8rem] rounded-lg bg-[#0096c7] hover:bg-[#0086b3]"
          }
        >
          <Users size={18} />
          <Label style={{ font: "inherit" }}>Участники проекта</Label>
        </Link>
        <Link
         href={`roles`}
          className={
            currentSetting === "roles"
              ? "hover:cursor-pointer text-white flex flex-row items-center gap-[0.5em] w-[80%] text-[1rem] font-normal my-[0.5rem] mx-0 py-[0.4rem] px-[0.8rem] rounded-lg bg-[#00b4d8]"
              : "hover:cursor-pointer text-white flex flex-row items-center gap-[0.5em] w-[80%] text-[1rem] font-normal my-[0.5rem] mx-0 py-[0.4rem] px-[0.8rem] rounded-lg bg-[#00b4d8] hover:bg-[#00aacc]"
          }
        >
          <Drama size={18} />
          <Label style={{ font: "inherit" }}>Роли</Label>
        </Link>
      </div>
      <div className="mt-[4rem] flex items-start justify-center">
        {children}
      </div>
    </div>
  );
}