﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace iCelerium.Models
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    using System.Data.Entity.Core.Objects;
    using System.Linq;
    
    public partial class SMSServersEntities : DbContext
    {
        public SMSServersEntities()
            : base("name=SMSServersEntities")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<C__MigrationHistory> C__MigrationHistory { get; set; }
        public virtual DbSet<AgentAssignClient> AgentAssignClients { get; set; }
        public virtual DbSet<AspNetRole> AspNetRoles { get; set; }
        public virtual DbSet<AspNetUserClaim> AspNetUserClaims { get; set; }
        public virtual DbSet<AspNetUserLogin> AspNetUserLogins { get; set; }
        public virtual DbSet<AspNetUser> AspNetUsers { get; set; }
        public virtual DbSet<AuditRecord> AuditRecords { get; set; }
        public virtual DbSet<Client> Clients { get; set; }
        public virtual DbSet<Commerciaux> Commerciauxes { get; set; }
        public virtual DbSet<MessageIn> MessageIns { get; set; }
        public virtual DbSet<MessageLog> MessageLogs { get; set; }
        public virtual DbSet<MessageOut> MessageOuts { get; set; }
        public virtual DbSet<TTransaction> TTransactions { get; set; }
        public virtual DbSet<TTransactionUpload> TTransactionUploads { get; set; }
        public virtual DbSet<UserProfile> UserProfiles { get; set; }
        public virtual DbSet<webpages_Membership> webpages_Membership { get; set; }
        public virtual DbSet<webpages_OAuthMembership> webpages_OAuthMembership { get; set; }
        public virtual DbSet<webpages_Roles> webpages_Roles { get; set; }
        public virtual DbSet<Zone> Zones { get; set; }
        public virtual DbSet<vTransaction> vTransactions { get; set; }
        public virtual DbSet<PostedTansaction> PostedTansactions { get; set; }
        public virtual DbSet<ClientsUpload> ClientsUploads { get; set; }
        public virtual DbSet<Tester> Testers { get; set; }
        public virtual DbSet<TerminalAssigned> TerminalAssigneds { get; set; }
        public virtual DbSet<Terminal> Terminals { get; set; }
        public virtual DbSet<TerminalType> TerminalTypes { get; set; }
        public virtual DbSet<Setting> Settings { get; set; }
        public virtual DbSet<Agent> Agents { get; set; }
        public virtual DbSet<CollationEntry> CollationEntries { get; set; }
        public virtual DbSet<Constituency> Constituencies { get; set; }
        public virtual DbSet<CreditType> CreditTypes { get; set; }
        public virtual DbSet<District> Districts { get; set; }
        public virtual DbSet<Echeance> Echeances { get; set; }
        public virtual DbSet<ElectoralArea> ElectoralAreas { get; set; }
        public virtual DbSet<Party> Parties { get; set; }
        public virtual DbSet<PollingStation> PollingStations { get; set; }
        public virtual DbSet<Region> Regions { get; set; }
        public virtual DbSet<Credit> Credits { get; set; }
    
        public virtual ObjectResult<spTransDay_Result> spTransDay(string date1)
        {
            var date1Parameter = date1 != null ?
                new ObjectParameter("date1", date1) :
                new ObjectParameter("date1", typeof(string));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<spTransDay_Result>("spTransDay", date1Parameter);
        }
    
        public virtual ObjectResult<spTransDayColPay_Result> spTransDayColPay(string date1, Nullable<int> key)
        {
            var date1Parameter = date1 != null ?
                new ObjectParameter("date1", date1) :
                new ObjectParameter("date1", typeof(string));
    
            var keyParameter = key.HasValue ?
                new ObjectParameter("key", key) :
                new ObjectParameter("key", typeof(int));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<spTransDayColPay_Result>("spTransDayColPay", date1Parameter, keyParameter);
        }
    
        public virtual ObjectResult<spTransPie_Result> spTransPie(string date1)
        {
            var date1Parameter = date1 != null ?
                new ObjectParameter("date1", date1) :
                new ObjectParameter("date1", typeof(string));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<spTransPie_Result>("spTransPie", date1Parameter);
        }
    
        public virtual ObjectResult<spToBePosted_Result> spToBePosted(Nullable<System.DateTime> date1, Nullable<System.DateTime> date2)
        {
            var date1Parameter = date1.HasValue ?
                new ObjectParameter("date1", date1) :
                new ObjectParameter("date1", typeof(System.DateTime));
    
            var date2Parameter = date2.HasValue ?
                new ObjectParameter("date2", date2) :
                new ObjectParameter("date2", typeof(System.DateTime));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<spToBePosted_Result>("spToBePosted", date1Parameter, date2Parameter);
        }
    
        public virtual ObjectResult<spToBePostedUploaded_Result> spToBePostedUploaded(Nullable<System.DateTime> date1, Nullable<System.DateTime> date2)
        {
            var date1Parameter = date1.HasValue ?
                new ObjectParameter("date1", date1) :
                new ObjectParameter("date1", typeof(System.DateTime));
    
            var date2Parameter = date2.HasValue ?
                new ObjectParameter("date2", date2) :
                new ObjectParameter("date2", typeof(System.DateTime));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<spToBePostedUploaded_Result>("spToBePostedUploaded", date1Parameter, date2Parameter);
        }
    
        public virtual ObjectResult<spToBeValidated_Result> spToBeValidated(Nullable<System.DateTime> date1, Nullable<System.DateTime> date2)
        {
            var date1Parameter = date1.HasValue ?
                new ObjectParameter("date1", date1) :
                new ObjectParameter("date1", typeof(System.DateTime));
    
            var date2Parameter = date2.HasValue ?
                new ObjectParameter("date2", date2) :
                new ObjectParameter("date2", typeof(System.DateTime));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<spToBeValidated_Result>("spToBeValidated", date1Parameter, date2Parameter);
        }
    
        public virtual int spUpdatePosted(Nullable<int> id)
        {
            var idParameter = id.HasValue ?
                new ObjectParameter("id", id) :
                new ObjectParameter("id", typeof(int));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("spUpdatePosted", idParameter);
        }
    
        public virtual ObjectResult<toRealign_Result> toRealign(string agentId, Nullable<System.DateTime> date)
        {
            var agentIdParameter = agentId != null ?
                new ObjectParameter("AgentId", agentId) :
                new ObjectParameter("AgentId", typeof(string));
    
            var dateParameter = date.HasValue ?
                new ObjectParameter("date", date) :
                new ObjectParameter("date", typeof(System.DateTime));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<toRealign_Result>("toRealign", agentIdParameter, dateParameter);
        }
    
        public virtual ObjectResult<Validations_Result> Validations(Nullable<System.DateTime> date1, Nullable<System.DateTime> date2, string userID)
        {
            var date1Parameter = date1.HasValue ?
                new ObjectParameter("date1", date1) :
                new ObjectParameter("date1", typeof(System.DateTime));
    
            var date2Parameter = date2.HasValue ?
                new ObjectParameter("date2", date2) :
                new ObjectParameter("date2", typeof(System.DateTime));
    
            var userIDParameter = userID != null ?
                new ObjectParameter("UserID", userID) :
                new ObjectParameter("UserID", typeof(string));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<Validations_Result>("Validations", date1Parameter, date2Parameter, userIDParameter);
        }
    }
}
