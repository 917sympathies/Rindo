"use client";
import React, { useState, useEffect } from "react";
import Header from "@/components/header";
import { usePathname, useParams } from "next/navigation";
import { getRights } from "@/requests";
import { Kanban, TableProperties } from "lucide-react";
import Link from "next/link";
import { UserRights } from "@/types";
import {Dialog, DialogContent} from "@/components/ui/dialog";
import {Label} from "@/components/ui/label";
import {Button} from "@/components/ui/button";
import AddTaskDialog from "@/components/addTaskDialog";

interface Props {
    children: React.ReactNode;
}

export default function Layout({ children }: Props) {
    const pathname = usePathname();
    const { id } = useParams<{ id: string }>();
    const [rights, setRights] = useState<UserRights>();
    const [isSelectorVisible, setIsSelectorVisible] = useState<boolean>(true);
    const [isChatActive, setIsChatActive] = useState<boolean>(false);
    const [isModalOpen, setIsModalOpen] = useState<boolean>(false);
    const [currentPage, setCurrentPage] = useState<string>(
        pathname.split("/")[3]
    );
    // const selectorSelected =
    //     "bg-white text-black py-0 px-[0.4rem] my-0 mx-[0.2rem] justify-center flex flex-row items-center border-b-[0.1px] border-b-black/10";
    // const selector =
    //     "text-[rgb(102,102,102)] py-0 px-[0.4rem] my-0 mx-[0.1rem] rounded-t-[6px] border-b-[0.1px] border-black border-opacity-[0.03] justify-center flex flex-row items-center hover:bg-[rgb(1,1,1)] hover:bg-opacity-[0.03] ease-in-out duration-300";
    useEffect(() => {
        loadRights();
    }, [id]);

    useEffect(() => {
        setCurrentPage(pathname.split("/")[3]);
    }, [pathname]);

    useEffect(() => {
        if (currentPage === "settings") setIsSelectorVisible(false);
    }, [currentPage]);

    const loadRights = async () => {
        const userId = localStorage.getItem("userId");
        const response = await getRights(id, userId!);
        const data = response.data;
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
                <div className="flex flex-row items-center justify-between w-full ease-out duration-300 p-[0.4rem]">
                    <div className="flex flex-row gap-2">
                        <Link href={`/project/${id}/board`}>
                            <div className={currentPage == "board" ?
                                "flex flex-row items-center px-1 rounded-md bg-gray-100 hover:bg-gray-50 ease-in-out duration-200" :
                                "flex flex-row items-center px-1 rounded-md bg-gray-100 hover:bg-gray-50 ease-in-out duration-200"
                            }>
                                <Kanban className="text-[rgb(59,130,246)]" size={16} />
                                <Label className="m-0 self-center p-[0.3rem]">Канбан</Label>
                            </div>
                        </Link>
                        <Link href={`/project/${id}/list`}>
                            <div className={currentPage == "list" ?
                                "flex flex-row items-center px-1 rounded-md bg-gray-100 hover:bg-gray-50 ease-in-out duration-200" :
                                "flex flex-row items-center px-1 rounded-md bg-gray-100 hover:bg-gray-50 ease-in-out duration-200"
                            }>
                                <TableProperties className="text-[rgb(59,130,246)]" size={16} />
                                <Label className="m-0 self-center p-[0.3rem]">Список</Label>
                            </div>
                        </Link>
                    </div>
                    <Button onClick={() => setIsModalOpen(true)}>Создать задачу</Button>
                </div>
            ) : (
                <div></div>
            )}
            {/* <Dialog open={isModalOpen} onOpenChange={setIsModalOpen}>
        <DialogContent>
          <AddUserModal onClose={() => setIsModalOpen(false)} />
        </DialogContent>
      </Dialog> */}
            {/* <Sheet key={"right"} open={isModalOpen}>
        <SheetContent className='h-screen top-0 right-0 left-auto mt-0 w-[500px] rounded-none' side={"right"}>
          <AddTaskModal
            onClose={() => setIsModalOpen(false)}
          // setFetch={setFetch}
          />
        </SheetContent>
      </Sheet> */}
            <Dialog open={isModalOpen}>
                <DialogContent>
                    <AddTaskDialog onClose={() => setIsModalOpen(false)}/>
                </DialogContent>
            </Dialog>
            {children}
        </div>
    );
}
