import {Stage, StageDto, StageShortInfo, StageType} from "@/types";


export class StageMapperService {
    static mapToClient = (stages: Stage[]): StageDto[] => {
        return stages.map(stage => {
            return {
                id: stage.id,
                customName: StageMapperService.mapStageName(stage.type, stage.customName),
                type: stage.type,
                index: stage.index,
                tasks: stage.tasks,
                projectId: stage.projectId
            };
        });
    }

    static mapStageShortInfo = (stages: Stage[]): StageShortInfo[] => {
        return stages.map(stage => ({
            stageId: stage.id,
            customName: StageMapperService.mapStageName(stage.type, stage.customName),
        }))
    }

    private static mapStageName = (stageType: StageType, customName?: string): string => {
        switch (stageType) {
            case StageType.ToDo:
                return "Запланировано";
            case StageType.InProgress:
                return "В процессе";
            case StageType.ReadyToTest:
                return "Готово к тестированию";
            case StageType.Testing:
                return "В тестировании";
            case StageType.Closed:
                return "Закрыто";
            case StageType.Custom:
                return customName ?? "";
            default:
                return "";
        }
    }
}