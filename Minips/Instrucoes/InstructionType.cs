using Minips.Instructions.Annotations;

namespace Minips.Instructions
{
    public enum InstructionType
    {
        [Format('R')]
        [Funct(0x20)]
        [Opcode(0)]
        add,

        [Format('I')]
        [Opcode(0x8)]
        addi,

        [Format('I')]
        [Opcode(0x9)]
        addiu,

        [Format('R')]
        [Funct(0x21)]
        [Opcode(0)]
        addu,

        [Format('R')]
        [Funct(0x24)]
        [Opcode(0)]
        and,

        [Format('I')]
        [Opcode(0xc)]
        andi,

        [Format('I')]
        [Opcode(0x4)]
        beq,

        [Format('I')]
        [Opcode(0x5)]
        bne,

        [Format('J')]
        [Opcode(0x2)]
        j,

        [Format('J')]
        [Opcode(0x3)]
        jal,

        [Format('R')]
        [Funct(0x08)]
        [Opcode(0)]
        jr,

        [Format('J')]
        [Opcode(0x24)]
        lbu,

        [Format('I')]
        [Opcode(0x25)]
        lhu,

        [Format('I')]
        [Opcode(0x30)]
        ll,

        [Format('I')]
        [Opcode(0xf)]
        lui,

        [Format('I')]
        [Opcode(0x23)]
        lw,

        [Format('R')]
        [Funct(0x27)]
        [Opcode(0)]
        nor,

        [Format('R')]
        [Funct(0x25)]
        [Opcode(0)]
        or,

        [Format('I')]
        [Opcode(0xd)]
        ori,

        [Format('R')]
        [Funct(0x2A)]
        [Opcode(0)]
        slt,

        [Format('I')]
        [Opcode(0xa)]
        slti,

        [Format('I')]
        [Opcode(0xb)]
        sltiu,

        [Format('R')]
        [Funct(0x2B)]
        [Opcode(0)]
        sltu,

        [Format('R')]
        [Funct(0x00)]
        [Opcode(0)]
        sll,

        [Format('R')]
        [Funct(0x02)]
        [Opcode(0)]
        srl,

        [Format('I')]
        [Opcode(0x28)]
        sb,

        [Format('I')]
        [Opcode(0x38)]
        sc,

        [Format('I')]
        [Opcode(0x29)]
        sh,

        [Format('I')]
        [Opcode(0x2b)]
        sw,

        [Format('R')]
        [Funct(0x22)]
        [Opcode(0)]
        sub,

        [Format('R')]
        [Funct(0x23)]
        [Opcode(0)]
        subu,

        [Format('R')]
        [Funct(0xC)]
        [Opcode(0)]
        syscall
    }
}
