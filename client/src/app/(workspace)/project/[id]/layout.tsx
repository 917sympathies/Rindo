"use client";
import { useState, useEffect } from "react";
import Header from "@/components/header";
import Link from "next/link";
import { usePathname, useParams } from "next/navigation";
import { Kanban, GanttChart, TableProperties } from "lucide-react";
import { Dialog, DialogContent } from "@/components/ui/dialog";
import { Button } from "@/components/ui/button";
import { CirclePlus } from "lucide-react";
import AddUserModal from "@/components/addUserModal";
import { IUserRights } from "@/types";
import { GetRights } from "@/requests";

interface Props {
  children: React.ReactNode;
}

export default function Layout({ children }: Props) {
  const pathname = usePathname();
  const { id } = useParams<{ id: string }>();
  const [rights, setRights] = useState<IUserRights>({} as IUserRights);
  const [isSelectorVisible, setIsSelectorVisible] = useState<boolean>(true);
  const [isChatActive, setIsChatActive] = useState<boolean>(false);
  const [isModalOpen, setIsModalOpen] = useState<boolean>(false);
  const [currentPage, setCurrentPage] = useState<string>(
    pathname.split("/")[3]
  );
  const selectorSelected =
    "bg-white text-black py-0 px-[0.4rem] my-0 mx-[0.2rem] justify-center flex flex-row items-center border-b-[0.1px] border-b-black/10";
  const selector =
    "text-[rgb(102,102,102)] py-0 px-[0.4rem] my-0 mx-[0.1rem] rounded-t-[6px] border-b-[0.1px] border-black border-opacity-[0.03] justify-center flex flex-row items-center hover:bg-[rgb(1,1,1)] hover:bg-opacity-[0.03] ease-in-out duration-300";
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
    const response = await GetRights(id, userId!);
    const data = await response.json();
    setRights(data);
  };

  return (
    <div className="flex flex-col ease-out duration-300">
      <Header
        setIsSelectorVisible={setIsSelectorVisible}
        isChatActive={isChatActive}
        setIsChatActive={setIsChatActive}
      />
      {isSelectorVisible ? (
        <div
          className={
            isChatActive
              ? "flex flex-row items-center justify-between w-[calc(100%-21vw)] ease-out duration-300"
              : "flex flex-row items-center justify-between w-full ease-out duration-300"
          }
        >
          <div className="flex flex-row justify-left w-fit my-[0.6rem] mx-[5rem] py-[0.4rem] px-0 rounded-[0.4rem]">
            <Link href={`/project/${id}/board`}>
              <div
                className={currentPage == "board" ? selectorSelected : selector}
              >
                <Kanban className="text-[rgb(59,130,246)]" size={16} />
                <p className="m-0 self-center p-[0.3rem]">Канбан</p>
              </div>
            </Link>
            <Link href={`/project/${id}/list`}>
              <div
                className={currentPage == "list" ? selectorSelected : selector}
              >
                <TableProperties className="text-[rgb(59,130,246)]" size={16} />
                <p className="m-0 self-center p-[0.3rem]">Список</p>
              </div>
            </Link>
            <Link href={`/project/${id}/gantt`}>
              <div
                className={currentPage == "gantt" ? selectorSelected : selector}
              >
                <GanttChart className="text-[rgb(59,130,246)]" size={16} />
                <p className="m-0 self-center p-[0.3rem]">Диаграмма Ганта</p>
              </div>
            </Link>
          </div>
          <div
            className={
              rights.canInviteUser ? "mr-4 flex items-center" : "invisible"
            }
            onClick={() => setIsModalOpen(true)}
          >
            <Button className="text-white bg-blue-500 hover:bg-blue-800 rounded-lg flex flex-row gap-2">
              <CirclePlus size={18} />
              <div>Пригласить пользователя</div>
            </Button>
          </div>
        </div>
      ) : (
        <div></div>
      )}
      <Dialog open={isModalOpen} onOpenChange={setIsModalOpen}>
        <DialogContent>
          <AddUserModal onClose={() => setIsModalOpen(false)} />
        </DialogContent>
      </Dialog>
      {children}
    </div>
  );
}
