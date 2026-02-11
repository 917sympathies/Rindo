"use client";
import { useState, useEffect, Dispatch, SetStateAction, useCallback } from "react";
import {TaskDto, UserRights, UserDto} from "@/types";
import { Dialog, DialogContent } from "@/components/ui/dialog";
import { Avatar } from "@/components/ui/avatar";
import { MessageCircle } from "lucide-react";
import { usePathname, useSearchParams } from "next/navigation";
import { useRouter } from "next/navigation";
import { EllipsisVertical } from "lucide-react";
import { getTasksCommentsAmount, getUser } from "@/requests";
import {Label} from "@/components/ui/label";
import EditTaskDialog from "@/components/editTaskDialog";

interface ITaskProps {
    task: TaskDto;
    setFetch: Dispatch<SetStateAction<boolean>>;
    rights: UserRights | undefined;
}

export default function Task({ task, setFetch, rights }: ITaskProps) {
    const searchParams = useSearchParams();
    const pathname = usePathname();
    const router = useRouter();
    const [responsibleUser, setResponsibleUser] = useState<UserDto | null>(null);
    const [commentsAmount, setCommentsAmount] = useState<number | null>(null);

    useEffect(() => {
        if (task.assignee && task.assignee.id !== null) loadUserInfo(task.assignee.id);
        getCommentsCount();
    }, []);

    const loadUserInfo = async (id: string) => {
        getUser(id)
            .then(res => res.data)
            .then(user => {
                setResponsibleUser(user);
            })
    };

    const getCommentsCount = async () => {
        const response = await getTasksCommentsAmount(task.taskId);
        const data = response.data;
        setCommentsAmount(data);
    };

    const handleOpenModal = useCallback(
        (open: boolean) => {
            const params = new URLSearchParams(searchParams.toString());
            if (open) params.set("task", task.taskId);
            else params.delete("task");
            return params.toString();
        },
        [searchParams]
    );

    return (
        <>
            <div className="bg-white rounded-lg w-full self-center shadow">
                <div className="flex justify-between items-center text-black text-[0.95rem] m-0 pt-[0.8rem] px-[1rem] pb-0 text-clip">
                    <Label>{task.taskNumber} - {task.name}</Label>
                    <div
                        className="h-fit w-fit flex justify-center p-[0.2rem] rounded-[6px] hover:cursor-pointer hover:bg-[rgba(1,1,1,0.03)] ease-in-out duration-200"
                        onClick={() => router.push(pathname + "?" + handleOpenModal(true))}
                    >
                        <EllipsisVertical size={16} color="rgb(102,102,102)" />
                    </div>
                </div>
                <div className="text-[0.7rem] my-[0.2rem] mx-[0.8rem]">
                    <textarea
                        value={task.description}
                        disabled
                        cols={30}
                        maxLength={100}
                        className="resize-none border-0 overflow-hidden h-auto bg-inherit"
                    ></textarea>
                </div>
                <div className="flex flex-row justify-between content-center pb-[0.4rem] my-0 mx-[0.4rem]">
                    {responsibleUser ? (
                        <div className="flex flex-row items-center gap-2">
                            <Avatar className="bg-[#4198FF] text-white w-[2.5vh] h-[2.5vh] text-[0.6rem] m-[0.1rem] ml-[0.4rem] flex justify-center items-center">
                                {responsibleUser?.firstName?.slice(0, 1)}
                                {responsibleUser?.lastName?.slice(0, 1)}
                            </Avatar>
                            <Label className="text-black">{responsibleUser?.firstName} {responsibleUser?.lastName}</Label>
                        </div>
                    ) : <div></div> }
                    <div className="flex flex-row justify-end items-center">
                        <MessageCircle size={16} />
                        <div className="mr-[0.4rem] text-[0.8rem] text-hidden overflow-hidden whitespace-nowrap">
                            {commentsAmount}
                        </div>
                    </div>
                </div>
            </div>
            <Dialog open={searchParams.get("task") === task.taskId}>
                <DialogContent className="w-[140vh]" onOpenAutoFocus={(e) => e.preventDefault()}>
                    <EditTaskDialog
                        onClose={() => router.push(pathname + "?" + handleOpenModal(false))}
                        setFetch={setFetch}
                        rights={rights}
                    />
                </DialogContent>
            </Dialog>
        </>
    );
}
