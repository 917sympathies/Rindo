import {TaskDto} from "@/types";

export interface UpdateProjectDto {
    projectId: string;
    name: string;
    description: string;
}

export interface ProjectTasks {
    projectId: string;
    projectName: string;
    tasks: TaskDto[];
}