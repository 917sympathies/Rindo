import styles from "./styles.module.css";
import type { PropsWithChildren } from "react";
import Sidebar from "@/components/sidebar";

export default function Layout({
  children,
}: PropsWithChildren<unknown>) {

  return (
    <div className={styles.main} style={{ gridTemplateColumns: "2fr 10fr" }}>
      <Sidebar
      />
      {children}
    </div>
  );
}
