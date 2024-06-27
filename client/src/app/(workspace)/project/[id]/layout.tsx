"use client";
import styles from "./layoutstyles.module.css";
import { useState, useEffect } from "react";
import Header from "@/components/header";
import Link from "next/link";
import { usePathname, useParams } from "next/navigation";
import {
  Kanban,
  GanttChart,
  TableProperties,
  UserRoundPlus,
} from "lucide-react";
import { Dialog, DialogContent } from "@/components/ui/dialog";
import { Button } from "@/components/ui/button";
import { CirclePlus } from "lucide-react";
import AddUserModal from "@/components/addUserModal";
import {
  Tooltip,
  TooltipContent,
  TooltipTrigger,
  TooltipProvider,
} from "@/components/ui/tooltip";
import { IUserRights } from "@/types";

interface Props {
  children: React.ReactNode;
}

export default function Layout({ children }: Props) {
  const pathname = usePathname();
  const { id } = useParams<{ id: string }>();
  const [rights, setRights] = useState<IUserRights>({} as IUserRights)
  const [isSelectorVisible, setIsSelectorVisible] = useState<boolean>(true);
  const [isChatActive, setIsChatActive] = useState<boolean>(false);
  const [isModalOpen, setIsModalOpen] = useState<boolean>(false);
  const [currentPage, setCurrentPage] = useState<string>(
    pathname.split("/")[3]
  );

  useEffect(() => {
    getRights();
  }, [id]);

  useEffect(() => {
    setCurrentPage(pathname.split("/")[3]);
  }, [pathname]);

  useEffect(() => {
    if (currentPage === "settings") setIsSelectorVisible(false);
  }, [currentPage]);

  const getRights = async () => {
    const userId = localStorage.getItem("userId");
    const response = await fetch(`http://localhost:5000/api/role/${id}/${userId}`, {
      method: "GET",
      headers: { "Content-Type": "application/json" },
      credentials: "include",
    });
    const data = await response.json();
    setRights(data)
    console.log(data)
  }


  return (
    <div
      style={{
        display: "flex",
        flexDirection: "column",
        transition: "all .3s ease-out",
      }}
    >
      <Header
        setIsSelectorVisible={setIsSelectorVisible}
        isChatActive={isChatActive}
        setIsChatActive={setIsChatActive}
      />
      {isSelectorVisible ? (
        <div
          style={{
            display: "flex",
            flexDirection: "row",
            alignItems: "center",
            justifyContent: "space-between",
            width: `${isChatActive ? "calc(100% - 21vw)" : "100%"}`,
            transition: "all .3s ease-out",
          }}
        >
          <div className={styles.selector}>
            <Link href={`/project/${id}/board`}>
              <div
                className={
                  currentPage == "board"
                    ? styles.selectoritemselected
                    : styles.selectoritem
                }
              >
                <Kanban style={{ color: "rgb(59 130 246)" }} size={16} />
                <p
                  style={{
                    margin: "0",
                    alignSelf: "center",
                    padding: "0.3rem",
                  }}
                >
                  Канбан
                </p>
              </div>
            </Link>
            <Link href={`/project/${id}/list`}>
              <div
                className={
                  currentPage == "list"
                    ? styles.selectoritemselected
                    : styles.selectoritem
                }
              >
                <TableProperties style={{ color: "rgb(59 130 246)" }} size={16} />
                <p
                  style={{
                    margin: "0",
                    alignSelf: "center",
                    padding: "0.3rem",
                  }}
                >
                  Список
                </p>
              </div>
            </Link>
            <Link href={`/project/${id}/gantt`}>
              <div
                className={
                  currentPage == "gantt"
                    ? styles.selectoritemselected
                    : styles.selectoritem
                }
              >
                <GanttChart style={{ color: "rgb(59 130 246)" }} size={16} />
                <p
                  style={{
                    margin: "0",
                    alignSelf: "center",
                    padding: "0.3rem",
                  }}
                >
                  Диаграмма Ганта
                </p>
              </div>
            </Link>
          </div>
          {/* <TooltipProvider>
            <Tooltip>
              <TooltipTrigger asChild>
                <div
                  className="mr-4 flex items-center"
                  onClick={() => setIsModalOpen(true)}
                >
                  <Button className="text-white bg-blue-500 hover:bg-blue-800 rounded-lg flex flex-row gap-2 flex">
                    <CirclePlus size={18} />
                    <div>Добавить пользователя</div>
                  </Button>
                </div>
              </TooltipTrigger>
              <TooltipContent asChild>
                <div className="text-gray-500">Добавить пользователя</div>
              </TooltipContent>
            </Tooltip>
          </TooltipProvider> */}
          
          {/* <div
            className="bg-[#01010108] rounded-full p-2 mr-6 flex flex-row gap-2"
            onClick={() => setIsModalOpen(true)}
          >
            <UserRoundPlus color="rgb(102, 102, 102)"/>
            <div className="text-gray-500">Добавить пользователя</div>
          </div> */}

          <div
            className={rights.canInviteUser ? "mr-4 flex items-center" : "invisible"}
            onClick={() => setIsModalOpen(true)}
          >
            <Button
              className="text-white bg-blue-500 hover:bg-blue-800 rounded-lg flex flex-row gap-2 flex">
              <CirclePlus size={18} />
              <div>Пригласить пользователя</div>
            </Button>
          </div>
        </div>
      ) : (
        <div></div>
      )}
      {/* <div> */}
      <Dialog open={isModalOpen} onOpenChange={setIsModalOpen}>
        <DialogContent>
          <AddUserModal onClose={() => setIsModalOpen(false)} />
        </DialogContent>
      </Dialog>
      {/* <Modal
          open={isModalOpen}
          sx={{
            display: "flex",
            justifyContent: "center",
            alignSelf: "center",
            alignContent: "center",
          }}
        >
          <AddUserModal onClose={() => setIsModalOpen(false)} />
        </Modal> */}
      {/* </div> */}
      {children}
    </div>
  );
}
