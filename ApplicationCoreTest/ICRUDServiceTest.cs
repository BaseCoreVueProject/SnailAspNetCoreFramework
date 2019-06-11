using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using ApplicationCore;
using ApplicationCore.Abstract;
using DAL;
using DALTest;
using DALTest.Entities;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace ApplicationCoreTest
{
    public class ICRUDServiceTest
    {
        private ICRUDService<Student> _crudService;
        private DbContext _db;
        public ICRUDServiceTest()
        {
            // ��������л������ڴ����ݿ����sqlserver���ݿ�
            //_db = DatabaseDbContextHelper.GetInMemoryDbContext();//��helper���ȡdbcontext
            _db = DatabaseDbContextHelper.GetSqlServerDbContext();//��helper���ȡdbcontext
            var repository = new EFRepository<Student>(_db);
            _crudService = new CRUDService<Student>(repository);
        }

        /// <summary>
        /// ��ѯ,���Ϊ��������
        /// </summary>
        [Fact]
        public void Query_SingleTableResult_Test()
        {
            var studentQuery = new StudentQuery();
            var result=_crudService.Query(studentQuery);
        }

        /// <summary>
        /// ��ѯ�����Ϊ�������
        /// </summary>
        [Fact]
        public void Query_MultiTableResult_Test()
        {
            var studentQuery = new StudentQuery();

            //�������ͽ��
            var anonymousResult = _crudService.Query(studentQuery);

            //�������ͽ��
            Expression<Func<Student, object>> selector = a => new StudentResultDto
            {
                Id = a.Id,
                BirthDay = a.BirthDay,
                Name = a.Name,
                TeamId = a.Team.Id,
                TeamName = a.Team.Name
            };
            var objectResult = _crudService.Query(studentQuery);
        }

        [Fact]
        public void QueryPage_Test()
        {
            var studentQueryPage = new StudentQueryPage() { PageIndex = 1, PageSize = 2 };
            var result = _crudService.QueryPage(studentQueryPage);
        }

        [Fact]
        public void Add_Test()
        {
            _crudService.Add(new StudentSaveDto
            {
                Id=100,
                Name="�ܾ�"
            });
            var added = _crudService.Find(100);
            Assert.NotNull(added);
            Assert.Equal(DateTime.Now.Date, added.BirthDay);
        }

        [Fact]
        public void Update_Test()
        {
            var entity = _crudService.FirstOrDefault();
            _crudService.Update(new StudentSaveDto
            {
                Id = entity.Id,
                BirthDay = new DateTime(2000, 12, 1),
                Name = "�����޸�",
                TeamId = entity.TeamId+100
            });
            var updated = _crudService.FirstOrDefault(a => a.Id == entity.Id);
            Assert.Equal(updated.BirthDay, new DateTime(2000, 12, 1));
            Assert.Equal(updated.Name, entity.Name);
            Assert.Equal(updated.TeamId, entity.TeamId);
        }

        [Fact]
        public void Delete_Test()
        {
            var entity = _crudService.FirstOrDefault();
            _crudService.Delete(entity.Id);
            var deleted = _crudService.FirstOrDefault(a => a.Id == entity.Id);
            Assert.Null(deleted);
        }
        #region �ڲ�dto
        /// <summary>
        /// ��ѯdto
        /// </summary>
        internal class StudentQuery : IQuery<Student, StudentResultDto>
        {
            public Expression<Func<Student, bool>> GeneratePredicateExpression()
            {
                //return a => a.Name!=null;
                return a => a.Team.Name.Contains("��");
            }

            public Func<IQueryable<Student>, IQueryable<Student>> IncludeFunc()
            {
                return a => a.Include(i => i.identityCard);
            }

            public Func<IQueryable<Student>, IQueryable<Student>> OrderFunc()
            {
                return a => a.OrderBy(i => i.Name);
            }
            public Expression<Func<Student, StudentResultDto>> SelectorExpression()
            {
                return a => new StudentResultDto
                {

                    Id = a.Id,
                    BirthDay = a.BirthDay,
                    Name = a.Name,
                    TeamId = a.Team.Id,
                    TeamName = a.Team.Name
                };
            }
        }

        /// <summary>
        /// ��ѯ���dto
        /// </summary>
        public class StudentResultDto
        {
            public int Id { get; set; }
            public string Name { get; set; }
            public DateTime BirthDay { get; set; }
            public int? TeamId { get; set; }
            public string TeamName { get; set; }
        }

        /// <summary>
        /// ��ҳ��ѯ 
        /// </summary>
        internal class StudentQueryPage : StudentQuery, IQueryPage<Student,StudentResultDto>
        {
            public int PageSize { get;set;}
            public int PageIndex { get;set;}
        }

        /// <summary>
        /// �������޸�dto
        /// </summary>
        internal class StudentSaveDto : ISaveDto<Student>
        {
            public int Id { get; set; }
            public string Name { get; set; }
            public DateTime BirthDay { get; set; }
            public int? TeamId { get; set; }
            public Student ConvertToEntity()
            {
                var student = new Student();
                student.Id = Id;
                student.Name = Name;
                student.BirthDay = BirthDay;
                student.TeamId = TeamId;
                if (BirthDay==default(DateTime))
                {
                    student.BirthDay = DateTime.Now.Date;
                }
                return student;
            }

            public List<string> GetUpdateProperties()
            {
                return new List<string> { nameof(BirthDay) };
            }
        }
        #endregion
    }

}
