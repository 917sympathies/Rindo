"use client";
import { useParams, notFound, redirect } from "next/navigation";
import Link from "next/link";
import { useEffect, useState } from "react";
import {TaskDto, StageType, AddStageDto, StageDto} from "@/types";
import { Sheet, SheetContent } from "../ui/sheet";
import { Button } from "../ui/button";
import { Input } from "../ui/input";
import {
    Droppable,
    Draggable,
    DragDropContext,
    DropResult,
} from "react-beautiful-dnd";
import AddTaskSheet from "../addTaskSheet";
import { UserRights, Stage as StageModel } from "@/types";
import { Label } from "../ui/label";
import { CirclePlus } from "lucide-react";
import { addStage, deleteStage, getRights, getStagesByProjectId, updateTaskStage } from "@/requests";
import Stage from "./stage";
import {StageMapperService} from "@/utils/stage-mapper.service";


export default function Kanban() {
    const { id } = useParams<{ id: string }>();
    const [stages, setStages] = useState<StageDto[] | null>(null);
    const [toFetch, setFetch] = useState<boolean>(false);
    const [isModalOpen, setIsModalOpen] = useState<boolean>(false);
    const [stageSelected, setStageSelected] = useState<string>("");
    const [newStageName, setNewStageName] = useState<string>("");
    const [rights, setRights] = useState<UserRights>();
    const [canAddStage, setCanAddStage] = useState<boolean>(
        stages?.length !== 3
    );

    useEffect(() => {
        getStages(id);
        loadRights();
    }, [id]);

    useEffect(() => {
        if (toFetch) {
            getStages(id);
            setFetch(false);
        }
    }, [toFetch]);

    const loadRights = async () => {
        const userId = localStorage.getItem("userId");
        const response = await getRights(id, userId!);
        const data = response.data;
        setRights(data)
    }

    const handleAddStage = async () => {
        setCanAddStage(false);
        const stage: AddStageDto = {
            customName: newStageName,
            projectId: id,
        };
        addStage(stage)
            .then(() => {
                setFetch(true);
            })
    }

    const handleDeleteStage = async (id: string) => {
        setCanAddStage(true);
        const response = await deleteStage(id);
        setFetch(true);
    }

    const getStages = async (id: string) => {
        getStagesByProjectId(id)
            .then(response => {
                const mappedStages = StageMapperService.mapToClient(response.data);
                setStages(mappedStages);
            });
    }

    async function onDragEnd({ source, destination }: DropResult) {
        if (!source || !destination || !stages) return;
        if (source.droppableId === destination.droppableId) return;
        const stageSource = stages.find(st => st.id === source.droppableId);
        if(!stageSource) return;
        const tasksSource = stageSource.tasks ?? [];
        const item: TaskDto = tasksSource.splice(source.index, 1)[0];
        const tasksDest = stages?.filter((st) => st.id === destination?.droppableId)[0];
        if (item && destination) item.stageId = destination.droppableId;
        if (tasksDest!.tasks === undefined) tasksDest!.tasks = Array(item);
        else tasksDest?.tasks.splice(destination!.index, 0, item);
        await updateTaskStage(item.taskId, destination.droppableId);
    }

    return (
        <div className="flex flex-row justify-start gap-4 flex-grow pb-2" style={{ paddingInlineStart: "0.4rem"}}>
            <DragDropContext onDragEnd={onDragEnd}>
                {stages &&
                    stages.map((stage) => (
                        <Stage
                            stage={stage}
                            key={stage.id}
                            onClick={() => {
                                setIsModalOpen(true);
                                setStageSelected(stage.id);
                            }}
                            handleDeleteStage={handleDeleteStage}
                            setFetch={setFetch}
                            rights={rights}
                        />
                    ))}
            </DragDropContext>
            {/*{stages && ((rights !== undefined) && (rights & UserRights.CanAddStage)) && stages.length !== 4 ? (*/}
            {/*    <div className="flex flex-col items-center">*/}
            {/*        <div className="flex flex-row items-center gap-1">*/}
            {/*            <Input*/}
            {/*                className="rounded-md text-center border-0 bg-background w-60 border focus-visible:outline-none focus-visible:ring-0 focus-visible:ring-slate-950 focus-visible:ring-offset-0 dark:bg-none"*/}
            {/*                onChange={(e) => setNewStageName(e.target.value)}*/}
            {/*                placeholder="Название стадии"*/}
            {/*            />*/}
            {/*            <Button className="p-3 rounded-md text-black bg-[rgba(102,153,255,0.6)] hover:bg-[rgba(102,153,255,0.5)] text-black/70" onClick={() => handleAddStage()}>*/}
            {/*                <CirclePlus size={16}></CirclePlus>*/}
            {/*            </Button>*/}
            {/*        </div>*/}
            {/*    </div>*/}
            {/*) : (*/}
            {/*    <div></div>*/}
            {/*)}*/}
            <Sheet key={"right"} open={isModalOpen}>
                <SheetContent className='h-screen top-0 right-0 left-auto mt-0 w-[500px] rounded-none' side={"right"}>
                    <AddTaskSheet
                        onClose={() => setIsModalOpen(false)}
                        stageId={stageSelected}
                        setFetch={setFetch}
                    />
                </SheetContent>
            </Sheet>
        </div>
    );
}
