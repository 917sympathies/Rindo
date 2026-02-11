"use client";
import { useCallback } from "react";
import { TaskDto, UserRights } from "@/types";
import {useParams, usePathname, useRouter, useSearchParams } from "next/navigation";
import { useState, useEffect } from "react";
import { Avatar } from "../ui/avatar";
import { Dialog, DialogContent } from "../ui/dialog";
import { EllipsisVertical } from "lucide-react";
import { Table, TableBody, TableCaption, TableCell, TableHead, TableHeader, TableRow } from "@/components/ui/table";
import { getRights, getTasksByProjectId } from "@/requests";
import { format } from "date-fns";
import EditTaskDialog from "@/components/editTaskDialog";

interface TaskListProps { }

export default function TaskList({ }: TaskListProps) {
    const { id } = useParams<{ id: string }>();
    const [toFetch, setFetch] = useState<boolean>(false);
    const [tasks, setTasks] = useState<TaskDto[]>([] as TaskDto[]);
    const [rights, setRights] = useState<UserRights>();
    const pathname = usePathname();
    const router = useRouter();
    const searchParams = useSearchParams();

    useEffect(() => {
        getTasks(id);
        loadRights();
    }, [id]);

    useEffect(() => {
        if (toFetch) {
            getTasks(id);
            setFetch(false);
        }
    }, [toFetch]);

    const loadRights = async () => {
        const userId = localStorage.getItem("userId");
        const response = await getRights(id, userId!);
        const data = response.data;
        setRights(data);
    };

    const getTasks = async (id: string) => {
        const response = await getTasksByProjectId(id);
        const data = response.data;
        console.log(data);
        setTasks(data);
    };

    const handleOpenModal = useCallback(
        (taskId?: string) => {
            console.log(taskId);
            const params = new URLSearchParams(searchParams.toString());
            if (taskId) params.set("task", taskId);
            else params.delete("task");
            return params.toString();
        },
        [searchParams]
    );

    return (
        <>
            <div className="px-20">
                <Table>
                    <TableCaption>Список задач проекта</TableCaption>
                    <TableHeader>
                        <TableRow className="w-full">
                            <TableHead>Имя</TableHead>
                            <TableHead>Дата создания</TableHead>
                            <TableHead>Дедлайн</TableHead>
                            <TableHead>Ответственный</TableHead>
                            <TableHead></TableHead>
                        </TableRow>
                    </TableHeader>
                    <TableBody>
                        {tasks?.map((task) => (
                            <TableRow key={task.taskId}>
                                <TableCell className="font-medium overflow-hidden">
                                    {task.name}
                                </TableCell>
                                <TableCell className="font-medium">
                                    {format(task.created, "dd.MM.yyyy")}
                                </TableCell>
                                <TableCell className="font-medium">
                                    {task.deadlineDate ? format(task.deadlineDate, "dd.MM.yyyy") : "-"}
                                </TableCell>
                                <TableCell className="flex flex-row items-center gap-2">
                                    {task.assignee ?
                                        <div className="flex flex-row items-center gap-2">
                                            <Avatar className="bg-[#4198FF] text-white w-[2.5vh] h-[2.5vh] text-[0.6rem] flex justify-center items-center">
                                                {task.assignee?.firstName?.slice(0, 1)}
                                                {task.assignee?.lastName?.slice(0, 1)}
                                            </Avatar>
                                            <div>{task.assignee?.firstName + " " + task.assignee?.lastName}</div>
                                        </div> :
                                        <div>
                                            Нет ответственного
                                        </div>}
                                </TableCell>
                                <TableCell className="w-[2rem]">
                                    <EllipsisVertical
                                        className="p-1 hover:bg-gray-100 rounded-full ease-in-out duration-300 text-gray-500"
                                        size={24}
                                        onClick={() =>
                                            router.push(pathname + "?" + handleOpenModal(task.taskId))
                                        }
                                    />
                                </TableCell>
                            </TableRow>
                        ))}
                    </TableBody>
                </Table>
            </div>
            <div>
                <Dialog open={searchParams.has("task")}>
                    <DialogContent>
                        <div className="flex justify-center self-center content-center">
                            <EditTaskDialog
                                onClose={() => router.push(pathname + "?" + handleOpenModal())}
                                setFetch={setFetch}
                                rights={rights}
                            ></EditTaskDialog>
                        </div>
                    </DialogContent>
                </Dialog>
            </div>
        </>
    );
}
