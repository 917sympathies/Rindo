import type { PropsWithChildren } from "react";
import Sidebar from "@/components/sidebar";

export default function Layout({ children }: PropsWithChildren) {

  return (
    <div className="max-w-[100vw] max-h-[100vh] w-[100vw] h-[100vh] grid grid-cols-[2fr_12fr] flex-nowrap">
      <Sidebar/>
      {children}
    </div>
  );
}
